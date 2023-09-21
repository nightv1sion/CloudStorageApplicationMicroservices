git subtree split -P Backend/Shared/Middlewares -b middlewares-shared

git remote add shared-middlewares-submodule https://github.com/nightv1sion/night-cloud-backend-middlewares-infrastructure

git push -u shared-middlewares-submodule middlewares-shared:master

git remote add shared-middlewares-submodule https://github.com/nightv1sion/night-cloud-backend-middlewares-infrastructure

git rm -r Backend/Shared/Middlewares

git add .

git commit -m "remove Backend/Shared/Middlewares"

rm Backend/Shared/Middlewares

git submodule add https://github.com/nightv1sion/night-cloud-backend-shared-middlewares Backend/Shared/Middlewares

git add .

git commit -m "shared middlewares submodule"