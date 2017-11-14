import { Component, Inject, Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/Rx';
import { Observable } from 'rxjs';

const getResourceList = 'api/admin/Resources';
const setResource = 'api/admin/Resources';
const setCash = 'api/admin/Cash';
const getCashList = 'api/admin/Cash';
const loadInitialValues = 'api/admin/LoadInitialValues';

export interface ResourceEntryDto {
    name: string;
    amount: number;
}

export interface CashEntryDto {
    name: string;
    amount: number;
    paper: boolean;
}

@Injectable() 
export class Api {
    constructor(private readonly http: Http,
        @Inject('BASE_URL') private readonly baseUrl: string) {
    }

    getResourceList(): Observable<ResourceEntryDto[]> {
        return this.invokeGet<ResourceEntryDto[]>(getResourceList);
    }

    getCashList(): Observable<CashEntryDto[]> {
        return this.invokeGet<CashEntryDto[]>(getCashList);
    }


    setResource(name: string, amount: number): Observable<number> {
        return this.invokePost<number>(setResource,
            {
                name: name,
                amount: amount
            });
    }
    
    setCash(name: string, amount: number): Observable<number> {
        return this.invokePost<number>(setCash,
            {
                name: name,
                amount: amount
            });
    }
    
    loadInitialValues(): Observable<void> {
        return this.invokePost<void>(loadInitialValues,
            {
                x: 1
            });
    }
    
    private invokeGet<TReturn>(url: string): Observable<TReturn> {
        let targetUrl = this.baseUrl + url;
        console.log(`Invoke GET ${targetUrl}`);
        return this.http.get(targetUrl).map(x => x.json() as TReturn);
    }

    private invokePost<TReturn>(url: string, body: any): Observable<TReturn> {
        let targetUrl = this.baseUrl + url;
        console.log(`Invoke POST ${targetUrl} by ${JSON.stringify(body)}`);
        return this.http.post(targetUrl, body).map(x => x.json() as TReturn);
    }
}