 function JsAction-GenDoc
{
	try
	{
		$project = Get-Project
		Build-Project($project)
		$url = Ge-Url($project)
		$url+= "JsAction?doc=1&data="
		echo 'Will listen at '
		echo $url
		$DTE.ExecuteCommand("Debug.Start")
		Start-Sleep -s 5
		$httpclient = New-Object 'System.Net.WebClient'
		$js = $httpclient.DownloadString($url)
		$DTE.ExecuteCommand("Debug.StopDebugging")
		$path =	[System.Io.Path]::Combine([System.Io.Path]::GetTempPath(),"JsActions.vsdoc.js")
		[System.IO.File]::WriteAllText($path,$js)
		try
		{
			$project.projectitems.item('Scripts').ProjectItems.Item('JsActions.vsdoc.js').Delete()
		}
		catch [Exception]
		{}

		$project.projectitems.item('Scripts').ProjectItems.AddFromFileCopy($path)
		echo 'Js file copied. JsAction command is over'
		echo 'Get out of here, now.'
		

	}
	catch [Exception]
	{
		$exception = $_.Exception
		Write-Host $exception
	}
}


function Build-Project($project)
{
    $configuration = $DTE.Solution.SolutionBuild.ActiveConfiguration.Name

    $DTE.Solution.SolutionBuild.BuildProject($configuration, $project.UniqueName, $true)

    if ($DTE.Solution.SolutionBuild.LastBuildInfo)
    {
        throw 'The project ''' + $project.Name + ''' failed to build.'
    }
}



function Get-Url($project)
{
	return $project.Properties.Item("WebApplication.BrowseURL").Value	
}

Export-ModuleMember JsAction-GenDoc