import { Component } from '@angular/core';
import { WeatherForecast, WeatherForecastClient } from '../dotnet5-superpowers-api.service';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[];

  constructor(
    private client: WeatherForecastClient
  ) {
    this.refresh();
  }

  refresh() {
    this.client.get().subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }
}
