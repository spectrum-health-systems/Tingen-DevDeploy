# Creating a new Tingen DevDeploy release

Once all code changes have been made:

1. Clean the solution
2. Rebuild the solution
3. Rename the current "\release\TingenDevDeploy.exe" to "\release\TingenDevDeploy-X.Y.Z.exe"
4. Move that file to "\release\older_versions\"
5. Publish the new release
6. Commit the changes to GitHub
7. Overwrite the current TingenDevDeploy executable on the Tingen web server with the new version.