cls

cd $PSScriptRoot

$id = (Get-Item -Path ".\..").Name

Get-ChildItem -Directory -Recurse -Include bin, obj, out | Remove-Item -Recurse -Force
dotnet pack $id -c Release --output .

$apikey = Read-Host -Prompt "Enter nuget.org API key with push permission" -AsSecureString
$apikey = (New-Object PSCredential "user",$apikey).GetNetworkCredential().Password

$nuget_file = (Get-ChildItem "$id.*.nupkg").Name
dotnet nuget push $nuget_file --source nuget.org --api-key $apikey --force-english-output

Remove-Item "$id.*.nupkg" -ErrorAction Continue
Remove-Item "$id.*.snupkg" -ErrorAction Continue