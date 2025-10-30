# Generate values-local.yaml from environment variables
function New-LocalValues([string]$envFilePath, [string]$outputPath) {
    if (Test-Path $envFilePath) {
        Write-Color -Text "Generating $outputPath from $envFilePath" -Color Green
        
        # Load environment variables
        $envVars = @{}
        Get-Content $envFilePath | ForEach-Object {
            if ($_ -match "^\s*([^#][^=]*)\s*=\s*(.*)\s*$") {
                $name = $matches[1].Trim()
                $value = $matches[2].Trim()
                
                # Remove quotes if present
                if ($value -match '^"(.*)"$' -or $value -match "^'(.*)'$") {
                    $value = $matches[1]
                }
                
                $envVars[$name] = $value
            }
        }
        
        # Generate values file
        $valuesContent = @"
# Auto-generated values from .env file
# Do not edit manually - this file is regenerated on each deployment

ghcr:
  enabled: true
  server: ghcr.io
  username: "$($envVars['GITHUB_USERNAME'])"
  password: "$($envVars['GITHUB_TOKEN'])"
"@
        
        $valuesContent | Out-File -FilePath $outputPath -Encoding UTF8
        Write-Color -Text "Generated local values file: $outputPath" -Color Green
        
        return $true
    }
    else {
        Write-Color -Text "Environment file not found: $envFilePath" -Color Yellow
        return $false
    }
}

# Example usage:
# New-LocalValues -envFilePath ".env" -outputPath "values-local.yaml"