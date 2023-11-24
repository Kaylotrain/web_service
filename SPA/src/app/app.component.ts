import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccontService } from './_service/accont.service';
import { User } from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'Citas App';

  constructor(private http: HttpClient, private accountService: AccontService) {}
  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if (!userString) return;
    const user: User = JSON.parse(userString); 
    this.accountService.setCurrentUser(user);
  }
}
