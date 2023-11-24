import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccontService } from '../_service/accont.service';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const accontService = inject(AccontService);
  const toastr = inject(ToastrService);

  
  return accontService.currentUser$.pipe(
    map(user => {
      if (user) return true;
      toastr.error("Por favor, inicie sesión para acceder a esta página.");
      return false;
    })
  );
};
