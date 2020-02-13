import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as WithdrawalStore from '../store/Withdrawal';

interface ILocalState
{
  withdrawalAmount: number;
}

// At runtime, Redux will merge together...
type WithdrawalProps =
WithdrawalStore.WithdrawalState // ... state we've requested from the Redux store
  & typeof WithdrawalStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps<{ /*startDateIndex: string*/ }>; // ... plus incoming routing parameters


class Withdrawal extends React.PureComponent<WithdrawalProps, ILocalState> {
  constructor(props: WithdrawalProps)
  {
    super(props);

    this.state = {
      withdrawalAmount: 0.00,
    };
  }

  private handleChange = (event: React.ChangeEvent<HTMLInputElement>)=> {
    const value = event.target.value;
    var parsedWithdrawalAmount = parseFloat(parseFloat(value).toFixed(2));
    this.setState({withdrawalAmount: parsedWithdrawalAmount});
  }

  public render() {
    return (
      <React.Fragment>
        <div>
          <h1>Withdrawal Amount</h1>
    
          <p>$<input type="number" step="0.01" min="0" value={this.state.withdrawalAmount} onChange={this.handleChange}/></p>
    
          <button onClick={() => this.props.submitWithdrawal(this.state.withdrawalAmount)}>Withdraw</button>
        </div>
        {this.renderWithdrawaling()}
      </React.Fragment>
    );
  }

  private renderWithdrawaling() {
    
    return (
      <div className="d-flex justify-content-between">
        {this.props.isLoading && <span>Depositing...</span>}
      </div>
    );
  }
}

export default connect(
  (state: ApplicationState) => state.withdrawal, // Selects which state properties are merged into the component's props
  WithdrawalStore.actionCreators // Selects which action creators are merged into the component's props
)(Withdrawal as any);
