cls

cd $PSScriptRoot

$id = ((Get-Item -Path ".\..").Name)
& MSBuild.exe /m /t:clean
& MSBuild.exe /m /t:restore
& MSBuild.exe /m /t:pack /p:Configuration=Release

$package_file = @(Get-ChildItem "$id\bin\Release\*.nupkg" -Exclude "*.symbols.*" | Sort-Object -Property CreationTime -Descending)[0]
$package_file.Name

& nuget.exe push $package_file.FullName -source nuget.org

$package_file | Remove-Item