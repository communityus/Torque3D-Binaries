$Core::windowIcon = "data/icon.png";
$Core::splashWindowImage = "data/splash.png";

// Display a splash window immediately to improve app responsiveness before
// engine is initialized and main window created.
displaySplashWindow($Core::splashWindowImage);

// Console does something.
setLogMode(6);

// Disable script trace.
trace(false);

// Set the name of our application
$appName = "Preview4_0";

//-----------------------------------------------------------------------------
// Load up scripts to initialise subsystems.
ModuleDatabase.setModuleExtension("module");
ModuleDatabase.scanModules( "core", false );
ModuleDatabase.LoadExplicit( "CoreModule" );

//-----------------------------------------------------------------------------
// Load any gameplay modules
ModuleDatabase.scanModules( "data", false );
ModuleDatabase.LoadGroup( "Game" );

//Finally, initialize the client/server structure
ModuleDatabase.LoadExplicit( "Core_ClientServer" );

//If nothing else set a main menu, try to do so now
if(!isObject(Canvas.getContent()))
{
   %mainMenuGUI = ProjectSettings.value("UI/mainMenuName");
   if (isObject( %mainMenuGUI ))
      Canvas.setContent( %mainMenuGUI );
}

closeSplashWindow();

echo("Engine initialized...");
