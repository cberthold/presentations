import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/Deposit';

const Deposit = props => (
  <div>
    <h1>Deposit Amount</h1>

    <p>$</p>

    <button onClick={() => props.submitDeposit(0.00)}>Deposit</button>
  </div>
);

export default connect(
  state => state.deposit,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(Deposit);
