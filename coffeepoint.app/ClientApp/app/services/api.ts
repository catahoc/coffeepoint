import { Component, Inject, Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/Rx';
import { Observable, Subscription } from 'rxjs';

const getResourceList = 'api/admin/Resources';
const setResource = 'api/admin/Resources';
const setCash = 'api/admin/Cash';
const getCashList = 'api/admin/Cash';
const loadInitialValues = 'api/admin/LoadInitialValues';
const getAllMoney = 'api/admin/GetAllMoney';
const coffee = 'api/Coffee';
const putCashItem = 'api/Coffee/PutCash';
const getMoneyBack = 'api/Coffee/GetMoneyBack';
const order = 'api/Coffee/Order';

export interface ResourceEntryDto {
    name: string;
    amount: number;
}

export interface CashEntryDto {
    name: string;
    amount: number;
    paper: boolean;
}

export interface CoffeeDto {
    name: string;
    cost: number;
    isAvailable: boolean;
    isEnoughMoney: boolean;
    isHaveExchange: boolean;
}

export interface CashItemDto {
    name: string;
}

export interface CoffeeScreenDto {
    coffees: CoffeeDto[];
    cashItems: CashItemDto[];
    currentAmount: number;
}

export interface MoneyBackEntryDto {
    name: string;
    count: number;
}

@Injectable() 
export class Api {
    performsRequest: boolean = false;

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
    
    getMoneyBack(): Observable<MoneyBackEntryDto[]> {
        return this.invokePost<MoneyBackEntryDto[]>(getMoneyBack, {});
    }
    
    setCash(name: string, amount: number): Observable<number> {
        return this.invokePost<number>(setCash,
            {
                name: name,
                amount: amount
            });
    }
    
    putCashItem(name: string): Observable<void> {
        return this.invokePost<void>(putCashItem,
            {
                name: name
            });
    }
    
    order(name: string): Observable<void> {
        return this.invokePost<void>(order,
            {
                name: name
            });
    }
    
    getState(): Observable<CoffeeScreenDto> {
        return this.invokeGet<CoffeeScreenDto>(coffee);
    }
    
    loadInitialValues(): Observable<void> {
        return this.invokePost<void>(loadInitialValues, {});
    }
    
    getAllMoney(): Observable<void> {
        return this.invokePost<void>(getAllMoney, {});
    }

    private coverPerforming<T>(observable: Observable<T>): Observable<T> {
        return new Observable(x => {
            this.performsRequest = true;
            const connectable = observable.publish();
            const s1 = connectable.subscribe(() => this.performsRequest = false, () => this.performsRequest = false);
            const s2 = connectable.subscribe(x);
            const s3 = connectable.connect();
            const s = new Subscription();
            s.add(s1);
            s.add(s2);
            s.add(s3);
            return s;
        });
    }
    
    private invokeGet<TReturn>(url: string): Observable<TReturn> {
        let targetUrl = this.baseUrl + url;
        console.log(`Invoke GET ${targetUrl}`);
        return this.coverPerforming(this.http.get(targetUrl).map(x => x.json() as TReturn));
    }

    private invokePost<TReturn>(url: string, body: any): Observable<TReturn> {
        let targetUrl = this.baseUrl + url;
        console.log(`Invoke POST ${targetUrl} by ${JSON.stringify(body)}`);
        return this.coverPerforming(this.http.post(targetUrl, body).map(x => x.json() as TReturn));
    }
}