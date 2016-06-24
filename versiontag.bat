git config user.email "%1@outlook.com"
git config user.name "%1"
git remote set-url origin https://%1:%2@github.com/wasteam/waslibs.git
git checkout %3
git tag -a "%PackageVersionTag%" -m "Version built. %PackageVersionTag%"
git push origin %PackageVersionTag%