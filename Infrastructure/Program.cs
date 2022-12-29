using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using System.Collections.Generic;
using Pulumi.AzureAD.Inputs;
using AzureAD = Pulumi.AzureAD;

return await Pulumi.Deployment.RunAsync(() =>
{

    var prefix = "dev";
    var appName = "therubble-angular";
    
    var serverApplication = new AzureAD.Application($"{prefix}-{appName}", new AzureAD.ApplicationArgs
    {
        
        SignInAudience = "AzureADandPersonalMicrosoftAccount",
        SinglePageApplication = new ApplicationSinglePageApplicationArgs
        {
            RedirectUris = "http://localhost:4200/"
        },
        DisplayName = "Azure AD Example Server",
        IdentifierUris =
        {
            $"api://{prefix}-{appName}"
        },
        RequiredResourceAccesses = new InputList<ApplicationRequiredResourceAccessArgs>()
        {
            
            new ApplicationRequiredResourceAccessArgs
            {
                ResourceAppId = "eabdc81e-56f6-4aeb-bae1-f0ce11e8dc2b",
                ResourceAccesses = new InputList<ApplicationRequiredResourceAccessResourceAccessArgs>()
                {
                    new ApplicationRequiredResourceAccessResourceAccessArgs()
                    {
                         Id = "b99dc45c-a43a-479b-a8d8-a76fcc4dc83a",
                         Type = "Scope"
                    }
                }
            }
        },
        Api = new AzureAD.Inputs.ApplicationApiArgs
        {
            
            RequestedAccessTokenVersion = 2,
            KnownClientApplications = new InputList<string>()
            {
                "eabdc81e-56f6-4aeb-bae1-f0ce11e8dc2b",
            }
            
        },
        
    });
    
    var applicationId = serverApplication.ApplicationId;
    

    // Export the primary key of the Storage Account
    return new Dictionary<string, object?>
    {
        ["ClientId"] = applicationId
    };
});