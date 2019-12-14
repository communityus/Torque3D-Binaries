function AssetBrowser::prepareImportImageAsset(%this, %assetItem)
{
   if(getAssetImportConfigValue("Images/GenerateMaterialOnImport", "1") == 1 && %assetItem.parentAssetItem $= "")
   {
      //First, see if this already has a suffix of some sort based on our import config logic. Many content pipeline tools like substance automatically appends them
      %foundSuffixType = ImportAssetWindow.parseImageSuffixes(%assetItem);
      
      if(%foundSuffixType $= "")
      {
         %noSuffixName = %assetItem.AssetName;
      }
      else
      {
         %suffixPos = strpos(strlwr(%assetItem.AssetName), strlwr(%assetItem.imageSuffixType), 0);
         %noSuffixName = getSubStr(%assetItem.AssetName, 0, %suffixPos);
      }
   
      //Check if our material already exists
      //First, lets double-check that we don't already have an
      %materialAsset = ImportAssetWindow.findImportingAssetByName(%noSuffixName);
      %cratedNewMaterial = false;
      
      if(%materialAsset == 0)
      {
         %filePath = %assetItem.filePath;
         if(%filePath !$= "")
            %materialAsset = AssetBrowser.addImportingAsset("Material", "", "", %noSuffixName);
            
         %materialAsset.filePath = filePath(%assetItem.filePath) @ "/" @ %noSuffixName;
            
         %cratedNewMaterial = true;
      }
      
      if(isObject(%materialAsset))
      {
         //Establish parentage
         %itemId = ImportAssetTree.findItemByObjectId(%assetItem);
         %materialItemId = ImportAssetTree.findItemByObjectId(%materialAsset);
         
         %assetItem.parentId = %materialItemId;
         %assetItem.parentAssetItem = %materialAsset;
         
         ImportAssetTree.reparentItem(%itemId, %materialItemId);
         
         ImportAssetWindow.assetHeirarchyChanged = true;
         
         ImportAssetTree.buildVisibleTree(true);
      }
      
      //Lets do some cleverness here. If we're generating a material we can parse like assets being imported(similar file names) but different suffixes
      //if we find these, we'll just populate into the original's material
      
      //If we need to append the diffuse suffix and indeed didn't find a suffix on the name, do that here
      if(%foundSuffixType $= "")
      {
         if(getAssetImportConfigValue("Images/UseDiffuseSuffixOnOriginImg", "1") == 1)
         {
            if(%foundSuffixType $= "")
            {
               %diffuseToken = getToken(getAssetImportConfigValue("Images/DiffuseTypeSuffixes", ""), ",", 0);
               %assetItem.AssetName = %assetItem.AssetName @ %diffuseToken;
            }
         }
         else
         {
            //We need to ensure that our image asset doesn't match the same name as the material asset, so if we're not trying to force the diffuse suffix
            //we'll give it a generic one
            if(%materialAsset.assetName $= %assetItem.assetName)
            {
               %assetItem.AssetName = %assetItem.AssetName @ "_image";
            }
         }
         
         %foundSuffixType = "diffuse";
      }
      
      if(%foundSuffixType !$= "")
      {
         //otherwise, if we have some sort of suffix, we'll want to figure out if we've already got an existing material, and should append to it  
         
         if(getAssetImportConfigValue("Materials/PopulateMaterialMaps", "1") == 1)
         {
            if(%foundSuffixType $= "diffuse")
               %materialAsset.diffuseImageAsset = %assetItem;
            else if(%foundSuffixType $= "normal")
               %materialAsset.normalImageAsset = %assetItem;
            else if(%foundSuffixType $= "metalness")
               %materialAsset.metalnessImageAsset = %assetItem;
            else if(%foundSuffixType $= "roughness")
               %materialAsset.roughnessImageAsset = %assetItem;
               else if(%foundSuffixType $= "specular")
               %materialAsset.specularImageAsset = %assetItem;
            else if(%foundSuffixType $= "AO")
               %materialAsset.AOImageAsset = %assetItem;
            else if(%foundSuffixType $= "composite")
               %materialAsset.compositeImageAsset = %assetItem;
         }
      }
      
      //If we JUST created this material, we need to do a process pass on it to do any other setup for it
      if(%cratedNewMaterial)
      {
         AssetBrowser.prepareImportMaterialAsset(%materialAsset);
      }
   }
   
   %assetItem.processed = true;
}

function AssetBrowser::importImageAsset(%this, %assetItem)
{
   %moduleName = AssetImportTargetModule.getText();
   
   %assetType = %assetItem.AssetType;
   %filePath = %assetItem.filePath;
   %assetName = %assetItem.assetName;
   %assetImportSuccessful = false;
   %assetId = %moduleName@":"@%assetName;
   
   %assetPath = AssetBrowser.dirHandler.currentAddress @ "/";
   
   %assetFullPath = %assetPath @ "/" @ fileName(%filePath);
   
   %newAsset = new ImageAsset()
   {
      assetName = %assetName;
      versionId = 1;
      imageFile = fileName(%filePath);
      originalFilePath = %filePath;
   };
   
   %assetImportSuccessful = TAMLWrite(%newAsset, %assetPath @ "/" @ %assetName @ ".asset.taml"); 
   
   //and copy the file into the relevent directory
   %doOverwrite = !AssetBrowser.isAssetReImport;
   if(!pathCopy(%filePath, %assetFullPath, %doOverwrite))
   {
      error("Unable to import asset: " @ %filePath);
      return;
   }
   
   %moduleDef = ModuleDatabase.findModule(%moduleName,1);
         
   if(!AssetBrowser.isAssetReImport)
      AssetDatabase.addDeclaredAsset(%moduleDef, %assetPath @ "/" @ %assetName @ ".asset.taml");
   else
      AssetDatabase.refreshAsset(%assetId);
}

function AssetBrowser::buildImageAssetPreview(%this, %assetDef, %previewData)
{
   %previewData.assetName = %assetDef.assetName;
   %previewData.assetPath = %assetDef.scriptFile;
   //%previewData.doubleClickCommand = "EditorOpenFileInTorsion( "@%previewData.assetPath@", 0 );";
   
   %imageFilePath = %assetDef.getImageFilename();
   if(isFile(%imageFilePath))
      %previewData.previewImage = %imageFilePath;
   else
      %previewData.previewImage = "core/rendering/images/unavailable";
   
   %previewData.assetFriendlyName = %assetDef.assetName;
   %previewData.assetDesc = %assetDef.description;
   %previewData.tooltip = %assetDef.friendlyName @ "\n" @ %assetDef;
}

function AssetBrowser::moveImageAsset(%this, %assetDef, %destination)
{
   %currentModule = AssetDatabase.getAssetModule(%assetDef.getAssetId());
   %targetModule = AssetBrowser.getModuleFromAddress(%destination);
   
   if(%currentModule $= %targetModule)
   {
      //just move the files  
      %assetPath = makeFullPath(AssetDatabase.getAssetFilePath(%assetDef.getAssetId()));
      %assetFilename = fileName(%assetPath);
      
      %newAssetPath = %destination @ "/" @ %assetFilename;
      
      %copiedSuccess = pathCopy(%assetPath, %destination @ "/" @ %assetFilename);
      %deleteSuccess = fileDelete(%assetPath);
      
      %imagePath = %assetDef.imageFile;
      %imageFilename = fileName(%imagePath);
      
      %copiedSuccess = pathCopy(%imagePath, %destination @ "/" @ %imageFilename);
      %deleteSuccess = fileDelete(%imagePath);
   }
   
   AssetDatabase.removeDeclaredAsset(%assetDef.getAssetId());
   AssetDatabase.addDeclaredAsset(%targetModule, %newAssetPath);
}

function GuiInspectorTypeImageAssetPtr::onControlDropped( %this, %payload, %position )
{
   Canvas.popDialog(EditorDragAndDropLayer);
   
   // Make sure this is a color swatch drag operation.
   if( !%payload.parentGroup.isInNamespaceHierarchy( "AssetPreviewControlType_AssetDrop" ) )
      return;

   %assetType = %payload.dragSourceControl.parentGroup.assetType;
   
   if(%assetType $= "ImageAsset")
   {
      echo("DROPPED A IMAGE ON AN IMAGE ASSET COMPONENT FIELD!");  
   }
   
   EWorldEditor.isDirty = true;
}