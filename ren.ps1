param ($oldNs, $newNs, $oldV, $newV)
$Utf8Encoding = New-Object System.Text.UTF8Encoding $True

Get-ChildItem * -Include *.cs, *.xaml, *csproj -recurse |
    Foreach-Object {
        $c = ($_ | Get-Content) 
        $c = $c -replace $oldNs, $newNs
        [IO.File]::WriteAllText($_.FullName, ($c -join "`r`n"), $Utf8Encoding)
    }

Get-ChildItem * -Include *.csproj -recurse |
    Foreach-Object {
        $c = ($_ | Get-Content) 
        $c = $c -replace "<Version>$oldV</Version>", "<Version>$newV</Version>"
        [IO.File]::WriteAllText($_.FullName, ($c -join "`r`n"), $Utf8Encoding)

    }

Get-ChildItem * -Include AssemblyInfo.cs -recurse |
    Foreach-Object {
        $c = ($_ | Get-Content) 
        $c = $c -replace "$oldV", "$newV"
        [IO.File]::WriteAllText($_.FullName, ($c -join "`r`n"), $Utf8Encoding)

    }

Get-ChildItem build/* -Include *.nuspec -recurse |
    Foreach-Object {
        $c = ($_ | Get-Content) 
        $c = $c -replace "$oldV", "$newV"
        [IO.File]::WriteAllText($_.FullName, ($c -join "`r`n"), $Utf8Encoding)

    }