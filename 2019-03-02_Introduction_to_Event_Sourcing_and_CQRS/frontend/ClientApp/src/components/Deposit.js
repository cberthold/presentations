import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/Deposit';

const Deposit = props => (
  <div>
    <h1>Deposit Amount</h1>

    <p>$</p>

    <button onClick={props.deposit}>Deposit</button>
  </div>
);

export default connect(
  state => state.deposits,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(Deposit);
