export const clientId = process.env.NEXT_PUBLIC_AZURE_B2C_CLIENT_ID;
export const tenantId = process.env.NEXT_PUBLIC_AZURE_B2C_TENANT_ID;
export const redirectUri = process.env.NEXT_PUBLIC_AZURE_B2C_REDIRECT_URL;
export const tenantName = process.env.NEXT_PUBLIC_AZURE_B2C_TENANT_NAME;

export const signUpSignIn =
  process.env.NEXT_PUBLIC_AZURE_B2C_POLICY_SIGNUP_SIGNIN;
export const forgotPassword =
  process.env.NEXT_PUBLIC_AZURE_B2C_POLICY_FORGOT_PASSWORD;

const policyNames = {
  signUpSignIn: signUpSignIn,
  forgotPassword: forgotPassword,
};

/**
 * Enter here the user flows and custom policies for your B2C application
 * To learn more about user flows, visit: https://docs.microsoft.com/en-us/azure/active-directory-b2c/user-flow-overview
 * To learn more about custom policies, visit: https://docs.microsoft.com/en-us/azure/active-directory-b2c/custom-policy-overview
 */
export const b2cPolicies = {
  authorities: {
    signUpSignIn: {
      authority: `https://${tenantName}.b2clogin.com/${tenantName}.onmicrosoft.com/${policyNames.signUpSignIn}`,
    },
    forgotPassword: {
      authority: `https://${tenantName}.b2clogin.com/${tenantName}.onmicrosoft.com/${policyNames.forgotPassword}`,
    },
  },
};

// Add here scopes for id token to be used at MS Identity Platform endpoints.
export const loginRequest = {
  scopes: ["User.Read"],
};

export const msalConfig = {
  auth: {
    clientId: clientId,
    authority: `https://login.microsoftonline.com/${tenantId}`,
    knownAuthorities: [`${tenantName}.b2clogin.com`], // [`${tenantName}.b2clogin.com`], // You must identify your tenant's domain as a known authority.
    redirectUri: redirectUri, // You must register this URI on Azure Portal/App Registration. Defaults to "window.location.href".
  },
};
