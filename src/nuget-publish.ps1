cls

cd $PSScriptRoot

$id = ((Get-Item -Path ".\..").Name)
iex "nuget pack -build -Prop Configuration=Release -sym $id\$id.csproj"

$package_file = @(Get-ChildItem "*.nupkg" -Exclude "*.symbols.*" | Sort-Object -Property CreationTime -Descending)[0]
$package_file.Name

iex ("nuget push " + $package_file.Name + " -source nuget.org")

Remove-Item *.nupkg