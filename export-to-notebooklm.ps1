$NotebookId = "9ce5520d-eff5-4484-be18-4c48a259d8b3"
$NlmPath = "C:\Users\Djawed\.local\bin\nlm.exe"
$ProjectRoot = "c:\Users\Djawed\Desktop\Parc Auto"
$MaxChars = 190000  # NotebookLM limit is ~200K chars per source

# -- Collect Backend files --
$backendContent = "# Parc Auto - Backend (.NET C#)`n`n"
$backendContent += "## Architecture: ASP.NET Core Web API with SQLite`n`n"

# Program.cs
$backendContent += "---`n### Program.cs (Entry Point)`n``````csharp`n"
$backendContent += (Get-Content "$ProjectRoot\Backend\Program.cs" -Raw)
$backendContent += "`n```````n`n"

# Models
$modelFiles = Get-ChildItem "$ProjectRoot\Backend\Models" -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue
foreach ($f in $modelFiles) {
    $rel = $f.FullName.Replace("$ProjectRoot\Backend\", "")
    $backendContent += "---`n### $rel`n``````csharp`n"
    $backendContent += (Get-Content $f.FullName -Raw)
    $backendContent += "`n```````n`n"
}

# Data (DbContext)
$dataFiles = Get-ChildItem "$ProjectRoot\Backend\Data" -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue
foreach ($f in $dataFiles) {
    $rel = $f.FullName.Replace("$ProjectRoot\Backend\", "")
    $backendContent += "---`n### $rel`n``````csharp`n"
    $backendContent += (Get-Content $f.FullName -Raw)
    $backendContent += "`n```````n`n"
}

# Helpers
$helperFiles = Get-ChildItem "$ProjectRoot\Backend\Helpers" -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue
foreach ($f in $helperFiles) {
    $rel = $f.FullName.Replace("$ProjectRoot\Backend\", "")
    $backendContent += "---`n### $rel`n``````csharp`n"
    $backendContent += (Get-Content $f.FullName -Raw)
    $backendContent += "`n```````n`n"
}

# Controllers
$controllerFiles = Get-ChildItem "$ProjectRoot\Backend\Controllers" -Filter "*.cs" -Recurse
foreach ($f in $controllerFiles) {
    $rel = $f.FullName.Replace("$ProjectRoot\Backend\", "")
    $backendContent += "---`n### $rel`n``````csharp`n"
    $backendContent += (Get-Content $f.FullName -Raw)
    $backendContent += "`n```````n`n"
}

# Save backend to file
$backendFile = "$ProjectRoot\notebooklm-export-backend.md"
$backendContent | Out-File -FilePath $backendFile -Encoding utf8

Write-Host "[OK] Backend export ready ($([math]::Round((Get-Item $backendFile).Length / 1KB)) KB)"

# -- Collect Frontend files into multiple chunks --
$extensions = @("*.ts", "*.html", "*.css")
$excludeDirs = @("node_modules", "dist", ".angular", "environments")

$frontendChunks = @()
$currentChunk = "# Parc Auto - Frontend (Angular) - Part 1`n`n"
$currentChunk += "## Architecture: Angular 19 SPA with standalone components`n`n"
$chunkNum = 1

foreach ($ext in $extensions) {
    $files = Get-ChildItem "$ProjectRoot\Frontend\src" -Filter $ext -Recurse -ErrorAction SilentlyContinue | 
        Where-Object { 
            $path = $_.FullName
            -not ($excludeDirs | Where-Object { $path -like "*\$_\*" })
        }
    
    foreach ($f in $files) {
        $rel = $f.FullName.Replace("$ProjectRoot\Frontend\src\", "")
        $lang = switch -Wildcard ($f.Extension) {
            ".ts"   { "typescript" }
            ".html" { "html" }
            ".css"  { "css" }
            default { "" }
        }
        $fileBlock = "---`n### $rel`n``````$lang`n"
        $fileBlock += (Get-Content $f.FullName -Raw)
        $fileBlock += "`n```````n`n"

        # Check if adding this file would exceed the limit
        if (($currentChunk.Length + $fileBlock.Length) -gt $MaxChars -and $currentChunk.Length -gt 200) {
            $frontendChunks += $currentChunk
            $chunkNum++
            $currentChunk = "# Parc Auto - Frontend (Angular) - Part $chunkNum`n`n"
        }
        $currentChunk += $fileBlock
    }
}
# Add the last chunk
if ($currentChunk.Length -gt 100) {
    $frontendChunks += $currentChunk
}

# Save frontend chunks to files
$frontendFiles = @()
for ($i = 0; $i -lt $frontendChunks.Count; $i++) {
    $partNum = $i + 1
    $filePath = "$ProjectRoot\notebooklm-export-frontend-part$partNum.md"
    $frontendChunks[$i] | Out-File -FilePath $filePath -Encoding utf8
    $frontendFiles += $filePath
    Write-Host "[OK] Frontend Part $partNum export ready ($([math]::Round((Get-Item $filePath).Length / 1KB)) KB, $($frontendChunks[$i].Length) chars)"
}

# Also save the combined version for reference
$frontendAll = $frontendChunks -join "`n`n"
$frontendAll | Out-File -FilePath "$ProjectRoot\notebooklm-export-frontend.md" -Encoding utf8

# -- User Guide --
$userGuideFile = "$ProjectRoot\notebooklm-export-userguide.txt"
if (Test-Path $userGuideFile) {
    Write-Host "[OK] User Guide export ready ($([math]::Round((Get-Item $userGuideFile).Length / 1KB)) KB)"
} else {
    Write-Host "[WARN] User Guide file not found at $userGuideFile - skipping"
}

# -- Clean existing sources --
Write-Host ""
Write-Host "Cleaning existing sources from notebook..."
$existingIds = & $NlmPath source list $NotebookId --quiet 2>$null
if ($existingIds) {
    $ids = $existingIds | Where-Object { $_.Trim() -ne "" }
    if ($ids.Count -gt 0) {
        Write-Host "  Deleting $($ids.Count) existing source(s)..."
        & $NlmPath source delete $ids --confirm
        Write-Host "  [OK] Cleaned."
    }
} else {
    Write-Host "  No existing sources found."
}

# -- Upload to NotebookLM --
Write-Host ""
Write-Host "Uploading Backend source to NotebookLM..."
Get-Content $backendFile -Raw | & $NlmPath source add $NotebookId --text - --title "Backend - .NET C# API" --wait

for ($i = 0; $i -lt $frontendFiles.Count; $i++) {
    $partNum = $i + 1
    $title = "Frontend - Angular App (Part $partNum/$($frontendFiles.Count))"
    Write-Host ""
    Write-Host "Uploading $title..."
    Get-Content $frontendFiles[$i] -Raw | & $NlmPath source add $NotebookId --text - --title $title --wait
}

if (Test-Path $userGuideFile) {
    Write-Host ""
    Write-Host "Uploading User Guide to NotebookLM..."
    Get-Content $userGuideFile -Raw | & $NlmPath source add $NotebookId --text - --title "Guide Utilisateur - Parc Auto" --wait
}

Write-Host ""
Write-Host "Done! Your project is now in NotebookLM."
Write-Host "Open: https://notebooklm.google.com/notebook/$NotebookId"
