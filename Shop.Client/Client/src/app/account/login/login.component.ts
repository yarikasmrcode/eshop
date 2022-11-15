import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { validate } from 'uuid';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm : FormGroup;
  returnUrl : string;

  constructor(private accountService : AccountService,private activatedRoutes : ActivatedRoute, private router : Router) { }

  ngOnInit(): void {
    this.returnUrl = this.activatedRoutes.snapshot.queryParams['returnUrl'] || '/shop';
    this.createLoginForm();
  }

  createLoginForm(){
    this.loginForm = new FormGroup({
      email : new FormControl('', [Validators.required, Validators.email]),
      password : new FormControl('', Validators.required)
    });
  }

  onSubmit(){
    this.accountService.login(this.loginForm.value).subscribe(() => {
      this.router.navigateByUrl(this.returnUrl);
    }, error => {
      console.log(error);
    })
  }

}
