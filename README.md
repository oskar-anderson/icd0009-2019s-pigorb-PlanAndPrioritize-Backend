Migrations

~~~
dotnet ef migrations --project DAL.App.EF --startup-project WebApp add Initial
dotnet ef database --project DAL.App.EF --startup-project WebApp update
dotnet ef database --project DAL.App.EF --startup-project WebApp drop
~~~

Install Microsoft.VisualStudio.Web.CodeGeneration.Design to WebApp
Api controllers
~~~
dotnet aspnet-codegenerator controller -name AppRolesController                     -m AppRole                     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name AppUsersController                     -m AppUser                     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name CategoriesController                   -m Category                    -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name CommentsController                     -m Comment                     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name FeaturesController                     -m Feature                     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name FeatureInVotingsController             -m FeatureInVoting             -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name FeatureStatusesController              -m FeatureStatus               -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name VotingsController                      -m Voting                      -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name VotingStatusesController               -m VotingStatus                -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name UserInVotingsController                -m UserInVoting                -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name UsersFeaturePrioritiesController       -m UsersFeaturePriority        -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name PriorityStatusesController             -m PriorityStatus              -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
~~~
