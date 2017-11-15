import { Component, Inject } from '@angular/core';
import { Api, CoffeeScreenDto, CoffeeDto, CashItemDto, MoneyBackEntryDto } from '../../services/api';

export enum ScreenState {
    Main,
    MoneyBack,
    CoffeeReady
}

@Component({
    selector: 'coffee',
    templateUrl: './coffee.component.html'
})
export class CoffeeComponent {
    state: ScreenState;
    ScreenState = ScreenState;
    screen: CoffeeScreenDto;
    coffee: string;
    gotBackMoney: MoneyBackEntryDto[] | null;

    constructor(private readonly api: Api) {
        this.updateState();
        this.state = ScreenState.Main;
    }

    public async updateState() {
        this.screen = await this.api.getState().toPromise();
    }

    async order(coffee: CoffeeDto) {
        await this.api.order(coffee.name).toPromise();
        this.coffee = coffee.name;
        this.state = ScreenState.CoffeeReady;
    }

    async goBack() {
        await this.updateState();
        this.state = ScreenState.Main;
    }

    async put(cashItem: CashItemDto) {
        await this.api.putCashItem(cashItem.name).toPromise();
        await this.updateState();
    }

    async getMyMoneyBack() {
        this.gotBackMoney = await this.api.getMoneyBack().toPromise();
        this.state = ScreenState.MoneyBack;
    }
}
