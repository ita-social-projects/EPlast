import React, { useState, useEffect } from 'react';
import ActionCard from '../ActionCard/ActionCard';
import http from '../../api/http';

const classes = require('./Actions.module.css');

const Actions = () => {

    const [actions, setActions] = useState([]);

    const updateActions = async () => {
        const actionsArray = await http.get('posts');
        setActions(actionsArray.data);
    }

    useEffect(() => {
        updateActions();
    }, []);

    const renderActions = (arr: any) => {
        if (arr) {
            const cutArr = arr.slice(0, 48);
            return cutArr.map((item: any) => (
                <ActionCard item={item} key={item.id} />
            ));
        }return null;
    };

    const plastActions = renderActions(actions);

    return (
        <div className={classes.background}>
            <h1 className={classes.mainTitle}>Акції</h1>
            <div className={classes.actionsWrapper}>{plastActions}</div>
        </div>

    )
}
export default Actions;
