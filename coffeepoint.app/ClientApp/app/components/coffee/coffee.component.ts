import { Component, Inject } from '@angular/core';
import { Api, CoffeeScreenDto, CoffeeDto, CashItemDto, MoneyBackEntryDto } from '../../services/api';

@Component({
    selector: 'coffee',
    templateUrl: './coffee.component.html'
})
export class CoffeeComponent {
    state: CoffeeScreenDto;
    gotBackMoney: MoneyBackEntryDto[] | null;

    constructor(private readonly api: Api) {
        this.updateState();
    }

    public async updateState() {
        this.state = await this.api.getState().toPromise();
    }

    async order(coffee: CoffeeDto) {
        await this.api.order(coffee.name).toPromise();
        await this.updateState();
    }

    async put(cashItem: CashItemDto) {
        await this.api.putCashItem(cashItem.name).toPromise();
        await this.updateState();
    }

    async getMyMoneyBack() {
        this.gotBackMoney = await this.api.getMoneyBack().toPromise();
        await this.updateState();
    }

    takeMoney() {
        this.gotBackMoney = null;
    }
}
