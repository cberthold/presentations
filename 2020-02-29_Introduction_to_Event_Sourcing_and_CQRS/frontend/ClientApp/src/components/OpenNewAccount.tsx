import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as OpenNewAccountStore from '../store/OpenNewAccount';

interface ILocalState
{
  firstName: string;
  lastName: string;
  accountType: string;
}

// At runtime, Redux will merge together...
type OpenNewAccountProps =
OpenNewAccountStore.OpenNewAccountState // ... state we've requested from the Redux store
  & typeof OpenNewAccountStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps<{ /*startDateIndex: string*/ }>; // ... plus incoming routing parameters


class OpenNewAccount extends React.PureComponent<OpenNewAccountProps, ILocalState> {
  constructor(props: OpenNewAccountProps)
  {
    super(props);

    this.state = {
      firstName: "",
      lastName: "",
      accountType: "Checking",
    };
  }

  private handleChange = (event: React.ChangeEvent<HTMLInputElement>)=> {
    const value = event.target.value;
    this.setState({
      ...this.state,
      [event.target.name]: value});
  }

  private handleSelectionChange = (event: React.ChangeEvent<HTMLSelectElement>)=> {
    const value = event.target.value;
    this.setState({
      ...this.state,
      [event.target.name]: value});
  }

  private openNewAccount = () => {
    const s = this.state;
    this.props.submitOpenNewAccount(s.firstName, s.lastName, s.accountType);
  }

  public render() {
    return (
      <React.Fragment>
        <div>
          <h1>Open a New Account</h1>
    
          <p>First Name: <input type="text" name="firstName" maxLength={30} value={this.state.firstName} onChange={this.handleChange}/></p>
          <p>Last Name: <input type="text" name="lastName" maxLength={30} value={this.state.lastName} onChange={this.handleChange}/></p>
          <p>Account Type: 
            <select name="accountType" onChange={this.handleSelectionChange} value={this.state.accountType}>
              <option value="Checking">Checking Plus More</option>
              <option value="Savings">Saving For The Future</option>
            </select>
          </p>
          <button onClick={() => this.openNewAccount()}>Withdraw</button>
        </div>
        {this.renderOpenNewAccounting()}
      </React.Fragment>
    );
  }

  private renderOpenNewAccounting() {
    
    return (
      <div className="d-flex justify-content-between">
        {this.props.isLoading && <span>Opening new account...</span>}
      </div>
    );
  }
}

export default connect(
  (state: ApplicationState) => state.openNewAccount, // Selects which state properties are merged into the component's props
  OpenNewAccountStore.actionCreators // Selects which action creators are merged into the component's props
)(OpenNewAccount as any);
