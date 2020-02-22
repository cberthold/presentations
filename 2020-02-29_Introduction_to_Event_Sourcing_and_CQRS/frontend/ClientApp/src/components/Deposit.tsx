import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import { ApplicationState } from '../store';
import * as DepositStore from '../store/Deposit';

interface ILocalState
{
  depositAmount: number;
}

// At runtime, Redux will merge together...
type DepositProps =
DepositStore.DepositState // ... state we've requested from the Redux store
  & typeof DepositStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps<{ /*startDateIndex: string*/ }>; // ... plus incoming routing parameters


class Deposit extends React.PureComponent<DepositProps, ILocalState> {
  constructor(props: DepositProps)
  {
    super(props);

    this.state = {
      depositAmount: 0.00,
    };
  }

  private handleChange = (event: React.ChangeEvent<HTMLInputElement>)=> {
    const value = event.target.value;
    var parsedDepositAmount = parseFloat(parseFloat(value).toFixed(2));
    this.setState({depositAmount: parsedDepositAmount});
  }

  public render() {
    return (
      <React.Fragment>
        <div>
          <h1>Deposit Amount</h1>
    
          <p>$<input type="number" step="0.01" min="0" value={this.state.depositAmount} onChange={this.handleChange}/></p>
    
          <button onClick={() => this.props.submitDeposit(this.state.depositAmount)}>Deposit</button>
        </div>
        {this.renderDepositing()}
      </React.Fragment>
    );
  }

  private renderDepositing() {
    
    return (
      <div className="d-flex justify-content-between">
        {this.props.isLoading && <span>Depositing...</span>}
      </div>
    );
  }
}

export default connect(
  (state: ApplicationState) => state.deposit, // Selects which state properties are merged into the component's props
  DepositStore.actionCreators // Selects which action creators are merged into the component's props
)(Deposit as any);
