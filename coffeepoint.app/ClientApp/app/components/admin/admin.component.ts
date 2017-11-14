import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import {Observable} from 'rxjs';

const getResourceList = 'api/Resources';
const incrementResource = 'api/Resources/IncrementResource';

interface ResourceEntryDto {
    name: string;
    amount: number;
    limit: number;
}

interface ResourceEntry {
    name: string;
    amount: number;
    limit: number;
    delta: number;
}

@Component({
    selector: 'admin',
    templateUrl: './admin.component.html'
})
export class AdminComponent {
    public resources: Promise<ResourceEntry[]>;

    constructor(private readonly http: Http,
                @Inject('BASE_URL') private readonly baseUrl: string) {
        this.resources = http.get(baseUrl + getResourceList)
            .map(x => (x.json() as ResourceEntryDto[]).map(this.makeEntry))
            .toPromise();
    }

    public async pushResource(entry: ResourceEntry) {
        try {
            await this.http.post(this.baseUrl + getResourceList,
                {
                    delta: entry.delta,
                    name: entry.name
                }).toPromise();
        } catch (e) {
            console.log(e);
        }
    }

    private makeEntry(dto: ResourceEntryDto): ResourceEntry {
        return {
            ...dto,
            delta: 0
        };
    }
}

interface WeatherForecast {
    dateFormatted: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}
