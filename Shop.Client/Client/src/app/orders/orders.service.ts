import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {

  baseUrl = 'https://localhost:7230/api/';

  constructor(private http : HttpClient) { }

  getOrdersForUser(){
    return this.http.get(this.baseUrl + 'orders');
  }

  getOrderDetailed(id : number){
    return this.http.get(this.baseUrl + 'orders/' + id);
  }
}
