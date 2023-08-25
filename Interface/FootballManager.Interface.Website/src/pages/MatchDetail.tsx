import { useEffect } from "react";
import { Button, Col, Row, Table, Alert, Form } from "react-bootstrap";
import moment from 'moment';
import { selectState, fetchAsync, rollingAsync, onCloseError, onCloseRivals, fetchMembersAsync, onShowAdd, MatchDetailProps, addMatchDetailAsync, deleteMatchDetailAsync, fetchMatchAsync } from '../slices/matchDetailSlice';
import { useAppDispatch, useAppSelector } from "../app/hooks";
import { useParams } from "react-router-dom";
import Rivals from "../components/Rivals";
import AddMatchDetail from "../components/AddMatchDetail";

export default function MatchDetail() {
    let { id } = useParams()
    moment.defaultFormat = 'YYYY-MM-DD HH:mm:ss'
    const state = useAppSelector(selectState)
    const dispatch = useAppDispatch()

    const fetch = () => {
        dispatch(fetchAsync({ id: parseInt(id ?? "") })).unwrap()
            .then(value => dispatch(fetchMembersAsync(0)).unwrap())
            .catch(ex => { console.log(ex) })
    }

    const rolling = () => {
        dispatch(rollingAsync({ id: parseInt(id ?? "") }))
            .unwrap()
            .then(values => {
            })
            .catch(error => {
            })
    }

    const onAddSubmit = (detail: MatchDetailProps) => {
        dispatch(onShowAdd(false))
        dispatch(addMatchDetailAsync(detail)).unwrap()
            .then(value => fetch())
            .catch(err => { })
    }

    const onDelete = (id: number) => {
        dispatch(deleteMatchDetailAsync(id)).unwrap()
            .then(value => fetch())
            .catch(err => { })
    }

    useEffect(() => {
        console.log('effect', state)
        dispatch(fetchMatchAsync({ id: parseInt(id ?? '') })).unwrap()
            .then(value => fetch())
            .catch(ex => { })
        return function cleanup() {
            console.log('unmount', state)
        }

    }, [])

    function isInvalidRivals() {
        if (state.match === undefined) {
            return true
        }
        if (state.data.length < state.match.teamCount * state.match.teamSize) {
            return true
        }
        return false
    }

    return (
        <>
            <h1>Match Detail</h1>
            {
                state.isLoading &&
                <Alert color='primary'>Loading...</Alert>
            }
            {
                state.isShowAdd &&
                <AddMatchDetail show={state.isShowAdd} matchId={parseInt(id ?? "")}
                    members={state.members} onSubmit={(detail) => { onAddSubmit(detail) }} onClose={() => dispatch(onShowAdd(false))} />
            }
            {
                state.isShowRivals && state.rolling &&
                <Rivals show={state.isShowRivals} rivals={state.rolling} onClose={() => dispatch(onCloseRivals())} />
            }
            {
                state.error &&
                <Alert show={state.error !== undefined} variant="danger" onClose={() => dispatch(onCloseError())} dismissible>
                    {state.error}
                </Alert>
            }
            <Row className="my-2">
                <Col className="d-flex justify-content-end">
                    <Button variant="primary" onClick={() => { dispatch(onShowAdd(true)) }}>Add Member</Button><div className="mx-2" />
                    <Button disabled={isInvalidRivals()} variant="success" onClick={() => { rolling() }}>Team Division Rivals</Button>
                </Col>
            </Row>

            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Match</th>
                        <th>Member</th>
                        <th>Is paid</th>
                        <th>Is Skip</th>
                        <th>Modified Date</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        state.data && state.data.map(value =>
                            <tr key={`key-${value.id}`}>
                                <td>{value.id}</td>
                                <td>{value.matchName}</td>
                                <td>{value.memberName}</td>
                                <td><Form.Check disabled checked={value.isPaid} type="switch" /></td>
                                <td><Form.Check disabled checked={value.isSkip} type="switch" /></td>
                                <td>{value.modifiedDate && moment(value.modifiedDate).format()}</td>
                                <td>
                                    <Button variant="warning" className="me-2" onClick={() => { }}>Edit</Button>
                                    <Button variant="danger" className="me-2" onClick={() => { onDelete(value.id) }}>Delete</Button>
                                </td>
                            </tr>
                        )
                    }
                </tbody>
            </Table>
        </>
    )
}