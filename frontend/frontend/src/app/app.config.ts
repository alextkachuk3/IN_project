import { provideRouter, Routes } from "@angular/router";
import { ApplicationConfig } from "@angular/core";

import { MainComponent } from "./main.component";
import { UploadComponent } from './upload/upload.component'
import { SignInComponent } from "./authorization/sign-in/sign-in.component"
import { SignUpComponent } from "./authorization/sign-up/sign-up.component"
import { PrivacyPolicyComponent } from "./privacy-policy.component";
import { NotFoundComponent } from "./not-found.component";
import { AuthGuard } from "./authorization/auth.guard";

const appRoutes: Routes = [
  { path: "", component: MainComponent, canActivate: [AuthGuard] },
  { path: "signin", component: SignInComponent, canActivate: [AuthGuard] },
  { path: "signup", component: SignUpComponent, canActivate: [AuthGuard] },
  { path: "upload", component: UploadComponent, canActivate: [AuthGuard] },
  { path: "privacy", component: PrivacyPolicyComponent },
  { path: "**", component: NotFoundComponent }
];

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(appRoutes)]
};
