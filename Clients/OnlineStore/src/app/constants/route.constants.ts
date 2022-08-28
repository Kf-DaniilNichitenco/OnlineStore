export class RouteConstants {
  public static accessDenied = "access-denied";
  public static login = "login";
  public static notFound = "404";
  public static auth = "auth";
  public static catalog = "catalog";

  public static signinRedirectCallback = `signin-callback`;
  public static signupRedirectCallback = `signup-callback`;
  public static signoutRedirectCallback = `signout-callback`;

  public static signinRedirectCallbackFullPath = `${RouteConstants.auth}/${RouteConstants.signinRedirectCallback}`;
  public static signupRedirectCallbackFullPath = `${RouteConstants.auth}/${RouteConstants.signupRedirectCallback}`;
  public static signoutRedirectCallbackFullPath = `${RouteConstants.auth}/${RouteConstants.signoutRedirectCallback}`;
}
