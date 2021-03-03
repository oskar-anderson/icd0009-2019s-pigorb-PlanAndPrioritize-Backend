Migrations

~~~
dotnet ef migrations --project DAL.App.EF --startup-project WebApp add Initial
dotnet ef database --project DAL.App.EF --startup-project WebApp update
dotnet ef database --project DAL.App.EF --startup-project WebApp drop
~~~


Remove cascade delete
~~~CSHARP
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // disable cascade delete initially for everything
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
~~~

Install Microsoft.VisualStudio.Web.CodeGeneration.Design to WebApp
MVC Web controllers (run inside WebApp)
~~~
dotnet aspnet-codegenerator controller -name AppRolesController                  -actions -m  AppRole                -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name AppUsersController                  -actions -m  AppUser                -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name CategoriesController                -actions -m  Category               -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name CommentsController                  -actions -m  Comment                -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name FeaturesController                  -actions -m  Feature                -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name FeatureInVotingsController          -actions -m  FeatureInVoting        -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name FeatureStatusesController           -actions -m  FeatureStatus          -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name VotingsController                   -actions -m  Voting                 -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name VotingStatusesController            -actions -m  VotingStatus           -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name UserInVotingsController             -actions -m  UserInVoting           -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name UsersFeaturePrioritiesController    -actions -m  UsersFeaturePriority   -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name PriorityStatusesController          -actions -m  PriorityStatus         -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
~~~

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

Solve the scaffolding problems on Ubuntu.
https://github.com/dotnet/Scaffolding/issues/1387#issuecomment-735289808

On project folder execute
~~~bash
mkdir Templates && mkdir Templates/ControllerGenerator && mkdir Templates/ViewGenerator
cp -r /home/$USER/.nuget/packages/microsoft.visualstudio.web.codegenerators.mvc/5.0.0/Templates/ControllerGenerator/* ./Templates/ControllerGenerator
cp -r /home/$USER/.nuget/packages/microsoft.visualstudio.web.codegenerators.mvc/5.0.0/Templates/ViewGenerator/* ./Templates/ViewGenerator/
~~~



Scaffold the existing database
~~~bash
dotnet ef dbcontext scaffold --project DAL.App.EF --startup-project WebApp "Server=barrel.itcollege.ee,1533;User Id=student;Password=Student.Bad.password.0;Database=akaver-distdemo01;MultipleActiveResultSets=true" Microsoft.EntityFrameworkCore.SqlServer --data-annotations --context AppDbContext --output-dir Models
~~~


Comments:
~~~
appsettings.json - change connectionstring for correct database

Dotnet framework - works only on the windows (latest 4.x)
Core works everywhere
Dotnet 5.0 is unified and works everywhere (5.0 is not LTS, version 6 will be)
~~~