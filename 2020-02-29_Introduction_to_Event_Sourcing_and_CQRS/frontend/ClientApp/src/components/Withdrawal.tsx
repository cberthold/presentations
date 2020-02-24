import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as WithdrawalStore from '../store/Withdrawal';
import * as AccountsStore from '../store/Accounts';
import { bindActionCreators, Dispatch } from 'redux';
import { AccountSelector } from './AccountSelector';
import { stat } from 'fs';

interface ILocalState
{
  withdrawalAmount: number;
}


// At runtime, Redux will merge together...
interface IWithdrawalProps
{
  withdrawal: WithdrawalStore.WithdrawalState;
  account: AccountsStore.AccountState;
  withdrawalActions: typeof WithdrawalStore.actionCreators,
  accountActions: typeof AccountsStore.actionCreators,
}
type WithdrawalProps =
  IWithdrawalProps
  & RouteComponentProps<{ /*startDateIndex: string*/ }>; // ... plus incoming routing parameters


class Withdrawal extends React.PureComponent<WithdrawalProps, ILocalState> {
  constructor(props: WithdrawalProps)
  {
    super(props);

    this.state = {
      withdrawalAmount: 0.00,
    };
  }

  public componentDidMount() {
    this.ensureDataFetched(true);
  }

  private ensureDataFetched(refresh: boolean) {
    this.props.accountActions.getAccounts(refresh);
  }

  private handleChange = (event: React.ChangeEvent<HTMLInputElement>)=> {
    const value = event.target.value;
    var parsedWithdrawalAmount = parseFloat(parseFloat(value).toFixed(2));
    this.setState({withdrawalAmount: parsedWithdrawalAmount});
  }


  private submitDepositForm(): void {
    const { withdrawalAmount } = this.state;
    const { submitWithdrawal } = this.props.withdrawalActions;
    const { selectedAccountId } = this.props.account;

    if(selectedAccountId == null)
    {
      alert('Select an account');
      return;
    }

    submitWithdrawal(selectedAccountId, withdrawalAmount);
  }

  public render() {
    const { accounts, selectedAccountId } = this.props.account;
    const { selectAccount } = this.props.accountActions;
    return (
      <React.Fragment>
        <div>
          <h1>Withdrawal Amount</h1>
          <p>Account: <AccountSelector accounts={accounts} onChange={selectAccount} value={selectedAccountId} /></p>
    
          <p>$<input type="number" step="0.01" min="0" value={this.state.withdrawalAmount} onChange={this.handleChange}/></p>
    
          <button onClick={() => this.submitDepositForm()}>Withdraw</button>
        </div>
        {this.renderWithdrawaling()}
      </React.Fragment>
    );
  }

  private renderWithdrawaling() {
    
    return (
      <div className="d-flex justify-content-between">
        {this.props.withdrawal.isLoading && <span>Withdrawal in progress...</span>}
      </div>
    );
  }
}

const mapDispatchToProps = (dispatch: Dispatch) =>
  ({
    withdrawalActions: bindActionCreators(WithdrawalStore.actionCreators, dispatch),
    accountActions: bindActionCreators(AccountsStore.actionCreators, dispatch),
  });

export default connect(
  (state: ApplicationState) => {
    const returnState = {
      withdrawal : state.withdrawal,
      account : state.account,
    }
    return returnState;
  },// Selects which state properties are merged into the component's props
  (dispatch: Dispatch) => {
    const returnProps = mapDispatchToProps(dispatch);
    return returnProps;
  }
  // Selects which action creators are merged into the component's props
)(Withdrawal as any);
