# GitHub Container Registry Authentication Setup

## Quick Setup

1. **Create a GitHub Personal Access Token:**

   - Go to GitHub → Settings → Developer settings → Personal access tokens → Tokens (classic)
   - Generate a new token with `read:packages` permission
   - Copy the token

2. **Configure your credentials:**

   - Edit the `.env` file in this directory
   - Add your GitHub username and token:
     ```
     GITHUB_USERNAME=your-github-username
     GITHUB_TOKEN=ghp_your-personal-access-token
     ```

3. **Run the deployment:**
   ```powershell
   .\helm-deploy-script.ps1 -includeSearchUI $true -includeSearchAPI $true
   ```

## Security Notes

- ✅ The `.env` file is ignored by git and won't be committed
- ✅ Never commit your personal access token to the repository
- ✅ The `.env.example` file shows the format without real credentials

## Alternative Methods

### Method 1: Environment Variables (Recommended)

Use the `.env` file as described above.

### Method 2: Script Parameters

Pass credentials directly (not recommended for public repos):

```powershell
.\helm-deploy-script.ps1 -githubUsername "your-username" -githubToken "your-token"
```

### Method 3: Manual Secret Creation

Create the Kubernetes secret manually:

```powershell
kubectl create secret docker-registry ghcr-secret --docker-server=ghcr.io --docker-username=your-username --docker-password=your-token -n s2search
```

## Troubleshooting

- If you see `ImagePullBackOff` errors, check that your token has `read:packages` permission
- Verify your username and token are correct in the `.env` file
- Make sure the `ghcr-secret` was created successfully in the `s2search` namespace
