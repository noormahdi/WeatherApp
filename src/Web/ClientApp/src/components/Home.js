import React, { Component } from 'react';
import { Weather } from './Weather';

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <div>
        <h1>Data Torque Tech Challenge</h1>
        <Weather></Weather>
      </div>
    );
  }
}
