import { NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, ErrorHandler, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NgFor],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  http = inject(HttpClient);
  title = 'DatingApp';
  users: any;
  
  ngOnInit(): void {
    // Http request in Angular
    // The get request NEEDS to have a subscribe, otherwise it doesn't do anything.
    this.http.get("https://localhost:5001/api/users").subscribe({
      // The subscribe takes different callback functions to help it understand what to do
      // Next tells the get request what to do next with the data
      next: users => this.users = users,
      // Error tells the get request what to do when an error occurs
      error: error => {console.log(error)},
      // Complete tells the get request what to do when the request has completed
      complete: () => console.log("Request at endpoint /api/users has completed")
    });
  }
}
