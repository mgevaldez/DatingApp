import {Component, inject, OnInit} from '@angular/core';
import {MembersService} from "../../_services/members.service";
import {MemberCardComponent} from "../member-card/member-card.component";
import {PaginationModule} from 'ngx-bootstrap/pagination';
import {AccountService} from "../../_services/account.service";
import {UserParams} from "../../_models/userParams";
import {FormsModule} from "@angular/forms";
import {ButtonRadioDirective, ButtonsModule} from "ngx-bootstrap/buttons";

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [
    MemberCardComponent,
    PaginationModule,
    FormsModule,
    ButtonRadioDirective
  ],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit{
  private accountService = inject(AccountService);
  protected memberService = inject(MembersService);
  userParams = new UserParams(this.accountService.currentUser());
  genderList = [
    { value: 'male', label: 'Males' },
    { value: 'female', label: 'Females' },
  ];

  ngOnInit(): void {
    if (!this.memberService.paginatedResult()) {
      this.loadMembers();
    }
  }

  loadMembers() {
    this.memberService.getMembers(this.userParams);
  }

  resetFilter() {
    this.userParams = new UserParams(this.accountService.currentUser());
    this.loadMembers();
  }

  pageChanged(event: any): void {
    if (this.userParams.pageNumber != event.page) {
      this.userParams.pageNumber = event.page;
      this.loadMembers();
    }
  }
}
