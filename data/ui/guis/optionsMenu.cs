//options settings

//Screen and Display menu
//Renderer Mode
//Screen resolution
//Windowed/fullscreen(borderless?)
//VSync

//Screen brightness
//screen brightness
//screen gamma

//Lighting Menu
//Shadow Distance(Distance shadows are drawn to. Also affects shadowmap slices)
//Shadow Quality(Resolution of shadows rendered, setting to none disables dynamic shadows)
//Soft Shadows(Whether shadow softening is used)
//Shadow caching(If the lights enable it, shadow caching is activated)
//Light Draw Distance(How far away lights are still drawn. Doesn't impact vector lights like the sun)

//Mesh and Textures Menu
//Draw distance(Overall draw distance) -slider
//Object draw distance(Draw distance from small/unimportant objects) -slider
//Mesh quality
//Texture quality
//Foliage draw distance
//Terrain Quality
//Decal Quality

//Effects Menu
//Parallax
//HDR
//Light shafts
//Motion Blur
//Depth of Field
//SSAO
//AA(ModelXAmount)[defualt is FXAA]
//Anisotropic filtering

//Keybinds

//Camera
//horizontal mouse sensitivity
//vert mouse sensitivity
//invert vertical
//zoom mouse sensitivities(both horz/vert)
//headbob
//FOV

function OptionsMenu::onWake(%this)
{
    OptionsMain.hidden = false;
    ControlsMenu.hidden = true;
    GraphicsMenu.hidden = true;
    AudioMenu.hidden = true;
    CameraMenu.hidden = true;
    ScreenBrightnessMenu.hidden = true;
    
    OptionsOKButton.hidden = false;
    OptionsCancelButton.hidden = false;
    OptionsDefaultsButton.hidden = false;
    
    OptionsMenu.tamlReader = new Taml();
    
    OptionsSettingStack.clear();
   
   %array = OptionsSettingStack;
   %array.clear();
   
   %keyboardMenuBtn = new GuiButtonCtrl(){
      text = "Keyboard and Mouse";
      profile = GuiMenuButtonProfile;
      extent = %array.extent.x SPC "35";
      command="ControlsMenu::loadSettings();";
   };
   
   %controllerMenuBtn = new GuiButtonCtrl(){
      text = "Controller";
      profile = GuiMenuButtonProfile;
      extent = %array.extent.x SPC "35";
      command="DisplayMenu::loadSettings();";
   };
   
   %displayMenuBtn = new GuiButtonCtrl(){
      text = "Display";
      profile = GuiMenuButtonProfile;
      extent = %array.extent.x SPC "35";
      command="DisplayMenu::loadSettings();";
   };
   
   %graphicsMenuBtn = new GuiButtonCtrl(){
      text = "Graphics";
      profile = GuiMenuButtonProfile;
      extent = %array.extent.x SPC "35";
      command="GraphicsMenu::loadSettings();";
   };
   
   %audioMenuBtn = new GuiButtonCtrl(){
      text = "Audio";
      profile = GuiMenuButtonProfile;
      extent = %array.extent.x SPC "35";
      command="AudioMenu::loadSettings();";
   };
   
   %gameplayMenuBtn = new GuiButtonCtrl(){
      text = "Gameplay";
      profile = GuiMenuButtonProfile;
      extent = %array.extent.x SPC "35";
      command="GameplayMenu::loadSettings();";
   };
   
   %array.add(%keyboardMenuBtn);
   //%array.add(%controllerMenuBtn);
   %array.add(%displayMenuBtn);
   %array.add(%graphicsMenuBtn);
   %array.add(%audioMenuBtn);
   //%array.add(%gameplayMenuBtn);
   
   //We programmatically set up our settings here so we can do some prepwork on the fields/controls
   //Presets
   /*OptionsMenu.addSettingOption(%array, "Preset", "High", ShadowQualityList, $pref::Video::Resolution);
   
   //AA
   OptionsMenu.addSettingOption(%array, "AntiAliasing", "FXAA 4x", ShadowQualityList, $pref::Video::Resolution);
   
   //Lighting
   OptionsMenu.addSettingOption(%array, "Shadow Quality", "High", ShadowQualityList, $pref::Video::Resolution);
   OptionsMenu.addSettingOption(%array, "Shadow Caching", "On", ShadowQualityList, $pref::Video::Resolution);
   OptionsMenu.addSettingOption(%array, "Soft Shadows", "High", ShadowQualityList, $pref::Video::Resolution);
   
   //Models and Textures
   OptionsMenu.addSettingOption(%array, "Level of Detail", "High", ShadowQualityList, $pref::Video::Resolution);
   OptionsMenu.addSettingOption(%array, "Texture Quality", "High", ShadowQualityList, $pref::Video::Resolution);
   OptionsMenu.addSettingOption(%array, "Material Quality", "High", ShadowQualityList, $pref::Video::Resolution);
   OptionsMenu.addSettingOption(%array, "Terrain Detail", "High", ShadowQualityList, $pref::Video::Resolution);
   OptionsMenu.addSettingOption(%array, "Decal Lifetime", "High", ShadowQualityList, $pref::Video::Resolution);
   OptionsMenu.addSettingOption(%array, "Ground Clutter Density", "High", ShadowQualityList, $pref::Video::Resolution);
   
   //Effects
   OptionsMenu.addSettingOption(%array, "HDR", "On", ShadowQualityList, $pref::Video::Resolution);
   OptionsMenu.addSettingOption(%array, "Parallax", "On", ShadowQualityList, $pref::Video::Resolution);
   OptionsMenu.addSettingOption(%array, "Ambient Occlusion", "On", ShadowQualityList, $pref::Video::Resolution);
   OptionsMenu.addSettingOption(%array, "Light Rays", "On", ShadowQualityList, $pref::Video::Resolution);
   OptionsMenu.addSettingOption(%array, "Depth of Field", "On", ShadowQualityList, $pref::Video::Resolution);
   OptionsMenu.addSettingOption(%array, "Vignetting", "On", ShadowQualityList, $pref::Video::Resolution);
   OptionsMenu.addSettingOption(%array, "Water Reflections", "On", ShadowQualityList, $pref::Video::Resolution);
   
   OptionsMenu.addSettingOption(%array, "Anisotropic Filtering", "16x", ShadowQualityList, $pref::Video::Resolution);*/
   
   if(!isObject(GraphicsSettingsCache))
   {
      new ArrayObject(GraphicsSettingsCache){};
   }
   
   GraphicsSettingsCache.empty();
}

function OptionsMenuOKButton::onClick(%this)
{
    //save the settings and then back out
    eval(OptionsMenu.currentMenu@"::apply();");
    OptionsMenu.backOut();
}

function OptionsMenuCancelButton::onClick(%this)
{
    //we don't save, so just back out of the menu 
    OptionsMenu.backOut();
}

function OptionsMenuDefaultsButton::onClick(%this)
{
    //we don't save, so go straight to backing out of the menu    
    eval(OptionsMenu.currentMenu@"::applyDefaults();");
    OptionsMenu.backOut();
}

function ControlsSettingsMenuButton::onClick(%this)
{
    OptionsMain.hidden = true;
    ControlsMenu.hidden = false;
    
    KeyboardControlPanel.hidden = false;
    MouseControlPanel.hidden = true;
    
    ControlsMenu.reload();
}

function GraphicsSettingsMenuButton::onClick(%this)
{
    OptionsMain.hidden = true;
    GraphicsMenu.hidden = false;
}

function CameraSettingsMenuButton::onClick(%this)
{
    OptionsMain.hidden = true;
    CameraMenu.hidden = false;
    
    CameraMenu.loadSettings();
}

function AudioSettingsMenuButton::onClick(%this)
{
    OptionsMain.hidden = true;
    AudioMenu.hidden = false;
    AudioMenu.loadSettings();
}

function ScreenBrSettingsMenuButton::onClick(%this)
{
    OptionsMain.hidden = true;
    ScreenBrightnessMenu.hidden = false;
}

function OptionsMenu::backOut(%this)
{
   //save the settings and then back out
   if(OptionsMain.hidden == false)
   {
      //we're not in a specific menu, so we're actually exiting
      Canvas.popDialog(OptionsMenu);

      if(isObject(OptionsMenu.returnGui) && OptionsMenu.returnGui.isMethod("onReturnTo"))
         OptionsMenu.returnGui.onReturnTo();
   }
   else
   {
      OptionsMain.hidden = false;
      ControlsMenu.hidden = true;
      GraphicsMenu.hidden = true;
      CameraMenu.hidden = true;
      AudioMenu.hidden = true;
      ScreenBrightnessMenu.hidden = true;
   }
}

function OptionsMenu::addSettingOption(%this, %arrayTarget, %optionName, %defaultValue, %settingsGroup, %targetVar)
{
    %option = TAMLRead("data/ui/guis/graphicsMenuSettingsCtrl.taml");
    
    if(!isMethod(%settingsGroup, "get") || !isMethod(%settingsGroup, "set"))
    {
      error("OptionsMenu::addSettingsOption - unrecognized settings group of: " @ %settingsGroup @ ". Did not have proper getter/setter");
      return "";
    }
    
    %option-->nameText.text = %optionName;
    %option-->SettingText.text = eval(%settingsGroup@"::"@"get();");
    %option.qualitySettingGroup = %settingsGroup;
    %option.targetVar = %targetVar;

    %arrayTarget.add(%option);

    return %option;
}

function OptionsMenu::addSliderOption(%this, %arrayTarget, %optionName, %variable, %range, %ticks, %value, %class)
{
    %option = TAMLRead("data/ui/guis/graphicsMenuSettingsSlider.taml");
    
    %option-->nameText.text = %optionName;

    %arrayTarget.add(%option);
    
    if(%range !$= "")
    {
       %option-->slider.range = %range;
    }
    
    if(%ticks !$= "")
    {
       %option-->slider.ticks = %ticks;
    }
    
    if(%variable !$= "")
    {
       %option-->slider.variable = %variable;
    }
    
    if(%value !$= "")
    {
       %option-->slider.setValue(%value);
    }
    
    if(%class !$= "")
    {
       %option-->slider.className = %class;
    }
    else
        %option-->slider.className = OptionsMenuSlider;
        
    %option-->slider.snap = true;
    
    %option-->slider.onValueSet();

    return %option;
}

function OptionsMenuSlider::onMouseDragged(%this)
{
   %this.onValueSet();
}

function OptionsMenuSlider::onValueSet(%this)
{
   %this.getParent().getParent()-->valueText.setText(mRound(%this.value * 10));  
}

function FOVOptionSlider::onMouseDragged(%this)
{
   %this.onValueSet();
}

function FOVOptionSlider::onValueSet(%this)
{
   %this.getParent().getParent()-->valueText.setText(mRound(%this.value));
}

//
function OptionsMenuForwardSetting::onClick(%this)
{
   //we need to advance through the value list, unless it's the end, in which case we do nothing  
   echo("Move forward in the list!");
   
   %settingCtrl = %this.getParent();
   %settingsList = eval(%settingCtrl.qualitySettingGroup@"::getList();");
   %settingsListCount = getTokenCount(%settingsList, ",");
   %currentSetting = %settingCtrl-->SettingText.text;
   
   //We consider 'custom' to be the defacto end of the list. The only way back is to go lower
   if(%currentSetting $= "Custom")
      return;
      
   %currentSettingIdx = OptionsMenu.getCurrentIndexFromSetting(%settingCtrl);
   
   if(%currentSettingIdx != %settingsListCount-1)
   {
      %currentSettingIdx++;
      
      //advance by one
      %newSetting = getToken(%settingsList, ",", %currentSettingIdx);
      eval(%settingCtrl.qualitySettingGroup@"::set(\""@%newSetting@"\");");
      %settingCtrl-->SettingText.setText( %newSetting );
      
      if(%currentSettingIdx == %settingsListCount)
      {
         //if we hit the end of the list, disable the forward button  
      }
   }
}

function OptionsMenuBackSetting::onClick(%this)
{
   //we need to advance through the value list, unless it's the end, in which case we do nothing  
   echo("Move back in the list!");
   
   %settingCtrl = %this.getParent();
   %settingsList = eval(%settingCtrl.qualitySettingGroup@"::getList();");
   %settingsListCount = getTokenCount(%settingsList, ",");
   %currentSetting = %settingCtrl-->SettingText.text;
   
   %currentSettingIdx = OptionsMenu.getCurrentIndexFromSetting(%settingCtrl);
   
   if(%currentSettingIdx != 0)
   {
      %currentSettingIdx--;
      
      //advance by one
      %newSetting = getToken(%settingsList, ",", %currentSettingIdx);
      eval(%settingCtrl.qualitySettingGroup@"::set(\""@%newSetting@"\");");
      %settingCtrl-->SettingText.setText( %newSetting );
      
      if(%currentSettingIdx == %settingsListCount)
      {
         //if we hit the end of the list, disable the forward button  
      }
   }
}

function OptionsMenu::getCurrentIndexFromSetting(%this, %settingCtrl)
{
   %settingsList = eval(%settingCtrl.qualitySettingGroup@"::getList();");
   %settingsListCount = getTokenCount(%settingsList, ",");
   %currentSetting = %settingCtrl-->SettingText.text;
   
   for ( %i=0; %i < %settingsListCount; %i++ )
   {
      %level = getToken(%settingsList, ",", %i);
      
      //find our current level
      if(%currentSetting $= %level)
      {
         return %i;
      }
   }
   
   return -1;
}

// =============================================================================
// CAMERA MENU
// =============================================================================
function CameraMenu::onWake(%this)
{
    
}

function CameraMenu::apply(%this)
{
   setFOV($pref::Player::defaultFov);  
}

function CameraMenu::loadSettings(%this)
{
   CameraMenuOptionsArray.clear();
   
   %option = OptionsMenu.addSettingOption(CameraMenuOptionsArray);
   %option-->nameText.setText("Invert Vertical");
   %option.qualitySettingGroup = InvertVerticalMouse;
   %option.init();
   
   %option = OptionsMenu.addSliderOption(CameraMenuOptionsArray, "0.1 1", 8, "$pref::Input::VertMouseSensitivity", $pref::Input::VertMouseSensitivity);
   %option-->nameText.setText("Vertical Sensitivity");
   
   %option = OptionsMenu.addSliderOption(CameraMenuOptionsArray, "0.1 1", 8, "$pref::Input::HorzMouseSensitivity", $pref::Input::HorzMouseSensitivity);
   %option-->nameText.setText("Horizontal Sensitivity");
   
   %option = OptionsMenu.addSliderOption(CameraMenuOptionsArray, "0.1 1", 8, "$pref::Input::ZoomVertMouseSensitivity", $pref::Input::ZoomVertMouseSensitivity);
   %option-->nameText.setText("Zoom Vertical Sensitivity");

   %option = OptionsMenu.addSliderOption(CameraMenuOptionsArray, "0.1 1", 8, "$pref::Input::ZoomHorzMouseSensitivity", $pref::Input::ZoomHorzMouseSensitivity);
   %option-->nameText.setText("Zoom Horizontal Sensitivity");
   
   %option = OptionsMenu.addSliderOption(CameraMenuOptionsArray, "65 90", 25, "$pref::Player::defaultFov", $pref::Player::defaultFov, FOVOptionSlider);
   %option-->nameText.setText("Field of View");
   
   CameraMenuOptionsArray.refresh();
}

function CameraMenuOKButton::onClick(%this)
{
   //save the settings and then back out
    CameraMenu.apply();
    OptionsMenu.backOut();
}

function CameraMenuDefaultsButton::onClick(%this)
{
   
}