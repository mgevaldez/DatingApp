import { ResolveFn } from '@angular/router';
import {Member} from "../_models/member";
import {MembersService} from "../_services/members.service";
import {inject} from "@angular/core";

export const memberDetailResolver: ResolveFn<Member | null> = (route, state) => {
  const memberService = inject(MembersService);
  const username = route.paramMap.get('username');

  if (!username) return null;

  return memberService.getMember(username);
};
