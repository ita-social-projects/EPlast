import React from 'react';
import { useParams } from 'react-router-dom';
import SortedEvents from './SortedEvents';


const classes = require('./ActionEvent.module.css');

const ActionEvent = () => {
    const { id } = useParams();
    return (
        <div className={classes.background}>
            <div className={classes.actionsWrapper}>
                <SortedEvents userId={id} />
            </div>
        </div>
    )
}
export default ActionEvent;