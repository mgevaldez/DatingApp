import { HttpInterceptorFn } from '@angular/common/http';
import {inject} from "@angular/core";
import {AccountService} from "../_services/account.service";

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const accountService = inject(AccountService);

  if (accountService.currentUser()) {
    const currentUser = accountService.currentUser();
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${currentUser?.token}`
      }
    });
  }
  return next(req);
};
