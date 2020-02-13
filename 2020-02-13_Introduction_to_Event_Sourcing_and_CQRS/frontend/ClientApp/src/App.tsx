import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import FetchData from './components/FetchData';
import Withdrawal from './components/Withdrawal';
import Deposit from './components/Deposit';
import BankTransactions from './components/BankTransactions';

import './custom.css'

export default () => (
    <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/fetch-data/:startDateIndex?' component={FetchData} />
        <Route path='/transactions' component={BankTransactions} />
        <Route path='/deposit' component={Deposit} />
        <Route path='/withdrawal' component={Withdrawal} />
    </Layout>
);
