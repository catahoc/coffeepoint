import { Component, Inject } from '@angular/core';
import { Api, ResourceEntryDto} from '../../services/api';

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

    constructor(private readonly api: Api) {
        this.resources = api.getResourceList()
            .map(x => x.map(this.makeEntry))
            .toPromise();
    }

    public async pushResource(entry: ResourceEntry) {
        try {
            entry.amount = await this.api.incrementResource(entry.name, entry.delta).toPromise();
        } catch (e) {
            console.log(e);
        }
    }

    public async popResource(entry: ResourceEntry) {
        try {
            entry.amount = await this.api.decrementResource(entry.name, entry.delta).toPromise();
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
