param($installPath, $toolsPath, $package, $project)
Import-Module (Join-Path $toolsPath JsAction-GenDoc.psd1) -ArgumentList $installPath