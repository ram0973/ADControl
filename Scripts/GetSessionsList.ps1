function Get-TSSessions {
    [CmdletBinding()]
    param(
        [Parameter()]
        [string]$ComputerName
    )
    
    Set-ExecutionPolicy -Scope Process -ExecutionPolicy Unrestricted;
    qwinsta /server:$ComputerName |
    # Parse output
    ForEach-Object {
        $_.Trim() -replace "\s+",","
    } |
    # Convert to objects
    ConvertFrom-Csv
}

Get-TSSessions
# Get-TSSessions -ComputerName "localhost" | ? { $_.State -eq 'Active' } | ft -AutoSize SessionName, UserName, ID