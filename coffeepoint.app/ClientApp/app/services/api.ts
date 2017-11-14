import { Component, Inject, Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/Rx';
import { Observable } from 'rxjs';

const getResourceList = 'api/Resources';
const getResource = 'api/Resources/';
const incrementResource = 'api/Resources/IncrementResourceCount';
const decrementResource = 'api/Resources/DecrementResourceCount';

export interface ResourceEntryDto {
    name: string;
    amount: number;
    limit: number;
}

@Injectable() 
export class Api {
    constructor(private readonly http: Http,
        @Inject('BASE_URL') private readonly baseUrl: string) {
    }

    getResourceList(): Observable<ResourceEntryDto[]> {
        return this.invokeGet<ResourceEntryDto[]>(getResourceList);
    }


    getResource(name: string): Observable<ResourceEntryDto> {
        return this.invokeGet<ResourceEntryDto>(getResource + name);
    }

    incrementResource(name: string, delta: number): Observable<number> {
        return this.invokePost<number>(incrementResource,
            {
                name: name,
                delta: delta
            });
    }

    decrementResource(name: string, delta: number): Observable<number> {
        return this.invokePost<number>(decrementResource,
            {
                name: name,
                delta: delta
            });
    }

    private invokeGet<TReturn>(url: string): Observable<TReturn> {
        let targetUrl = this.baseUrl + url;
        console.log(`Invoke GET ${targetUrl}`);
        return this.http.get(targetUrl).map(x => x.json() as TReturn);
    }

    private invokePost<TReturn>(url: string, body: any): Observable<TReturn> {
        let targetUrl = this.baseUrl + url;
        console.log(`Invoke POST ${targetUrl} by ${body}`);
        return this.http.post(targetUrl, body).map(x => x.json() as TReturn);
    }
}