import { Component, Inject } from '@angular/core';
import { Api, ResourceEntryDto, CashEntryDto} from '../../services/api';

@Component({
    selector: 'admin',
    templateUrl: './admin.component.html'
})
export class AdminComponent {
    public resources: Promise<ResourceEntryDto[]>;
    public cash: Promise<CashEntryDto[]>;

    constructor(private readonly api: Api) {
        this.load();
    }

    public async loadInitialResources() {
        await this.api.loadInitialValues().toPromise();
        await this.load();
    }

    public async setResource(entry: ResourceEntryDto, newAmount: number) {
        try {
            await this.api.setResource(entry.name, newAmount).toPromise();
            entry.amount = newAmount;
        } catch (e) {
            console.log(e);
        }
    }

    public async setCash(entry: CashEntryDto, newAmount: number) {
        try {
            await this.api.setCash(entry.name, newAmount).toPromise();
            entry.amount = newAmount;
        } catch (e) {
            console.log(e);
        }
    }

    private async load() {
        this.resources = this.api.getResourceList()
            .toPromise();
        this.cash = this.api.getCashList()
            .toPromise();
        await Promise.all([this.resources, this.cash]);
    }
}