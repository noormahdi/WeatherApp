import { Component } from 'react';
import { WeatherClient, WeatherForecast } from '../web-api-client';
import './Weather.css'
export class Weather extends Component {
  static displayName = Weather.name;

  constructor(props) {
    super(props);
    this.state = { forecasts: WeatherForecast, loading: true };
  }

  componentDidMount() {
    this.populateWeatherData();
  }

  static renderForecastsTable(forecasts) {
    return (
      <div id="weather">
        <div className='temperature'>{forecasts.temperatureC}° C</div>
        <div className='wind'>{forecasts.windSpeedKmph} km/h</div>
        <div className='condition'>{forecasts.condition}</div>
        <div className='recommendation'>{forecasts.recommendation}</div>
      </div>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : Weather.renderForecastsTable(this.state.forecasts);

    return (
      <div>
        <h2>Weather forecast for Wellington, NZ</h2>
        {contents}
      </div>
    );
  }

  async populateWeatherData() {
    let client = new WeatherClient();
    const beehiveLat = -41.2787765;
    const beehiveLong = 174.7767745;
    const data = await client.getWeatherForecasts(beehiveLat,beehiveLong);
    this.setState({ forecasts: data, loading: false });
  }
}
