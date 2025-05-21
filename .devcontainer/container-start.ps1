# Note:
# This is run during container startup.

# Configure the Azure CLI
az config set core.output=table
az config set core.collect_telemetry=no

# Posh Install
curl -s https://ohmyposh.dev/install.sh | bash -s

$devcontainerProfilePath = @(
    '/home/vscode/.config/powershell/Microsoft.VSCode_profile.ps1',
    '/home/vscode/.config/powershell/Microsoft.PowerShell_profile.ps1'
)

foreach ($path in $devcontainerProfilePath) {
    New-Item -Path $path -ItemType File -Force;

    Add-Content -Path $path `
        -Value '/home/vscode/.local/bin/oh-my-posh init pwsh --config /home/vscode/.cache/oh-my-posh/themes/pixelrobots.omp.json | Invoke-Expression; function Get-All { Get-ChildItem -Force | Format-Table Mode, LastWriteTime, Length, Name }; Set-Alias ls Get-All;'
}

pwsh;