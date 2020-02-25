import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as DepositStore from '../store/Deposit';
import * as AccountsStore from '../store/Accounts';
import { AccountSelector } from './AccountSelector';
import { Dispatch, bindActionCreators } from 'redux';

interface ILocalState
{
  depositAmount: number;
}

interface IDepositProps
{
  deposit: DepositStore.DepositState;
  account: AccountsStore.AccountState;
  depositActions: typeof DepositStore.actionCreators,
  accountActions: typeof AccountsStore.actionCreators,
}
type DepositProps =
  IDepositProps
  & RouteComponentProps<{ /*startDateIndex: string*/ }>; // ... plus incoming routing parameters


class Deposit extends React.PureComponent<DepositProps, ILocalState> {
  constructor(props: DepositProps)
  {
    super(props);

    this.state = {
      depositAmount: 0.00,
    };
  }

  // This method is called when the component is first added to the document
  public componentDidMount() {
    this.ensureDataFetched(true);
  }

  private ensureDataFetched(refresh: boolean) {
    this.props.accountActions.getAccounts(refresh);
  }

  private handleChange = (event: React.ChangeEvent<HTMLInputElement>)=> {
    const value = event.target.value;
    var parsedDepositAmount = parseFloat(parseFloat(value).toFixed(2));
    this.setState({depositAmount: parsedDepositAmount});
  }

  private submitDepositForm(): void {
    const { depositAmount } = this.state;
    const { selectedAccountId } = this.props.account;
    const { submitDeposit } = this.props.depositActions;

    if(selectedAccountId == null)
    {
      alert('Select an account');
      return;
    }

    submitDeposit(selectedAccountId, depositAmount)
  }

  public render() {
    const { accounts, selectedAccountId } = this.props.account;
    const { selectAccount } = this.props.accountActions;
    return (
      <React.Fragment>
        <div>
          <h1>Deposit Amount</h1>
          <p>Account: <AccountSelector accounts={accounts} onChange={selectAccount} value={selectedAccountId} /></p>
    
          <p>$<input type="number" step="0.01" min="0" value={this.state.depositAmount} onChange={this.handleChange}/></p>
    
          <button onClick={() => this.submitDepositForm()}>Deposit</button>
        </div>
        {this.renderDepositing()}
      </React.Fragment>
    );
  }

  private renderDepositing() {
    
    return (
      <div className="d-flex justify-content-between">
        {this.props.deposit.isLoading && <span>Depositing...</span>}
      </div>
    );
  }
}

const mapDispatchToProps = (dispatch: Dispatch) =>
  ({
    depositActions: bindActionCreators(DepositStore.actionCreators, dispatch),
    accountActions: bindActionCreators(AccountsStore.actionCreators, dispatch),
  });

export default connect(
  (state: ApplicationState) => {
    const returnState = {
      deposit : state.deposit,
      account : state.account,
    }
    return returnState;
  },// Selects which state properties are merged into the component's props
  (dispatch: Dispatch) => {
    const returnProps = mapDispatchToProps(dispatch);
    return returnProps;
  }// Selects which action creators are merged into the component's props
)(Deposit as any);
