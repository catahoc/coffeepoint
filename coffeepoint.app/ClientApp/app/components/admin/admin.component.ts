import { Component, Inject } from '@angular/core';
import { Api, ResourceEntryDto, CashEntryDto} from '../../services/api';

@Component({
    selector: 'admin',
    templateUrl: './admin.component.html'
})
export class AdminComponent {
    public resources: ResourceEntryDto[];
    public cash: CashEntryDto[];

    constructor(private readonly api: Api) {
        this.load();
    }

    public async loadInitialResources() {
        await this.api.loadInitialValues().toPromise();
        await this.load();
    }

    public async getAllMoney() {
        await this.api.getAllMoney().toPromise();
        await this.load();
    }

    public async setResource(entry: ResourceEntryDto, newAmount: number) {
        await this.api.setResource(entry.name, newAmount).toPromise();
        await this.load();
    }

    public async setCash(entry: CashEntryDto, newAmount: number) {
        await this.api.setCash(entry.name, newAmount).toPromise();
        await this.load();
    }

    private async load() {
        this.resources = await this.api.getResourceList().toPromise();
        this.cash = await this.api.getCashList().toPromise();
    }
}