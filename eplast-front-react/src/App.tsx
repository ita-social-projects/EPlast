import React from 'react';
import './App.css';
import { Provider } from 'react-redux';
import { PersistGate } from 'redux-persist/integration/react';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import { store, persistor } from './store';
import SignUp from './pages/SignUp/SignUp';
import SignIn from './pages/SignIn/SignIn';
import 'antd/dist/antd.css';
import HeaderContainer from './components/Header/HeaderContainer';
import Home from './pages/Home/Home';
import DecisionTable from './pages/DecisionTable/DecisionTable';
import FooterContainer from './components/Footer/FooterContainer';
import Contacts from './pages/Contacts/Contacts';
import Actions from './pages/Actions/Actions';
import ActionEvent from './pages/Actions/ActionEvent/ActionEvent';

function App() {
  return (
    <div className="App">
      <Provider store={store}>
        <PersistGate loading={null} persistor={persistor}>
          <Router>
            <HeaderContainer />
            <div className="mainContent">
              <Switch>
                <Route exact path="/" component={Home} />
                <Route path="/signup" component={SignUp} />
                <Route path="/signin" component={SignIn} />
                <Route path="/decisions" component={DecisionTable} />
                <Route path="/contacts" component={Contacts} />
                <Route exact path="/actions" component={Actions} />
                <Route exact path="/actions/events/:id" component={ActionEvent} />
              </Switch>
            </div>
            <FooterContainer />
          </Router>
        </PersistGate>
      </Provider>
    </div>
  );
}

export default App;
